import { initializeApp, getApps } from 'firebase/app';
import { getMessaging, getToken, isSupported, onMessage, type Messaging } from 'firebase/messaging';
import apiClient from '../api/apiClient';

type AppUser = {
    userId?: string;
    UserId?: string;
    id?: string;
    Id?: string;
    roleName?: string;
    RoleName?: string;
    Roles?: string[];
    roles?: string[];
};

const firebaseConfig = {
    apiKey: import.meta.env.VITE_FIREBASE_API_KEY,
    authDomain: import.meta.env.VITE_FIREBASE_AUTH_DOMAIN,
    projectId: import.meta.env.VITE_FIREBASE_PROJECT_ID,
    storageBucket: import.meta.env.VITE_FIREBASE_STORAGE_BUCKET,
    messagingSenderId: import.meta.env.VITE_FIREBASE_MESSAGING_SENDER_ID,
    appId: import.meta.env.VITE_FIREBASE_APP_ID,
};

const vapidKey = import.meta.env.VITE_FIREBASE_VAPID_KEY;

let messagingInstance: Messaging | null = null;
let foregroundListenerBound = false;
let pendingInit: Promise<void> | null = null;

const isFirebaseConfigured = () =>
    Boolean(
        firebaseConfig.apiKey &&
        firebaseConfig.projectId &&
        firebaseConfig.messagingSenderId &&
        firebaseConfig.appId &&
        vapidKey
    );

const resolveUserId = (user?: AppUser | null) => user?.userId || user?.UserId || user?.id || user?.Id || '';

const resolveRole = (user?: AppUser | null) =>
    (user?.roleName || user?.RoleName || user?.Roles?.[0] || user?.roles?.[0] || '').toLowerCase();

const shouldEnablePushForUser = (user?: AppUser | null) => {
    const role = resolveRole(user);
    return role === 'driver' || role === 'customer' || role === 'transporter' ;
};

const getFirebaseApp = () => {
    if (!getApps().length) {
        initializeApp(firebaseConfig);
    }

    return getApps()[0];
};

const bindForegroundListener = async () => {
    if (foregroundListenerBound || !messagingInstance) {
        return;
    }

    onMessage(messagingInstance, (payload) => {
        if (Notification.permission !== 'granted') {
            return;
        }

        const title = payload.notification?.title || 'Navgatix';
        const body = payload.notification?.body || 'You have a new update.';
        new Notification(title, { body, icon: '/logo.png' });
    });

    foregroundListenerBound = true;
};

export const enablePushNotifications = async (user?: AppUser | null) => {
    if (pendingInit) {
        return pendingInit;
    }

    pendingInit = (async () => {
        try {
            if (!user || !shouldEnablePushForUser(user) || !isFirebaseConfigured()) {
                return;
            }

            if (!(await isSupported()) || !('serviceWorker' in navigator) || !('Notification' in window)) {
                return;
            }

            const permission = await Notification.requestPermission();
            if (permission !== 'granted') {
                return;
            }

            const registration = await navigator.serviceWorker.register(
                new URL('../firebase-messaging-sw.ts', import.meta.url),
                { type: 'module' }
            );

            const app = getFirebaseApp();
            messagingInstance = getMessaging(app);
            await bindForegroundListener();

            const token = await getToken(messagingInstance, {
                vapidKey,
                serviceWorkerRegistration: registration,
            });

            if (!token) {
                return;
            }

            const userId = resolveUserId(user);
            if (!userId) {
                return;
            }

            localStorage.setItem('pushDeviceToken', token);
            await apiClient.post('/PushNotifications/device-token', {
                userId,
                deviceToken: token,
                platform: 'web',
                deviceId: navigator.userAgent,
            });
        } catch (error) {
            console.error('Push notification setup failed.', error);
        } finally {
            pendingInit = null;
        }
    })();

    return pendingInit;
};

export const disablePushNotifications = async () => {
    const token = localStorage.getItem('pushDeviceToken');
    const userStr = localStorage.getItem('user');
    const user = userStr ? (JSON.parse(userStr) as AppUser) : null;
    const userId = resolveUserId(user);

    if (!token || !userId) {
        return;
    }

    try {
        await apiClient.delete('/PushNotifications/device-token', {
            data: {
                userId,
                deviceToken: token,
            },
        });
    } catch (error) {
        console.error('Push notification cleanup failed.', error);
    } finally {
        localStorage.removeItem('pushDeviceToken');
    }
};
