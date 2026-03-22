import { initializeApp, getApps } from 'firebase/app';
import {
    getAuth,
    GoogleAuthProvider,
    createUserWithEmailAndPassword,
    sendEmailVerification,
    signInWithEmailAndPassword,
    signInWithPopup,
    sendPasswordResetEmail,
    reload,
    signOut,
    type User,
} from 'firebase/auth';

const firebaseConfig = {
    apiKey: import.meta.env.VITE_FIREBASE_API_KEY,
    authDomain: import.meta.env.VITE_FIREBASE_AUTH_DOMAIN,
    projectId: import.meta.env.VITE_FIREBASE_PROJECT_ID,
    storageBucket: import.meta.env.VITE_FIREBASE_STORAGE_BUCKET,
    messagingSenderId: import.meta.env.VITE_FIREBASE_MESSAGING_SENDER_ID,
    appId: import.meta.env.VITE_FIREBASE_APP_ID,
};

const getFirebaseApp = () => {
    if (!getApps().length) {
        initializeApp(firebaseConfig);
    }

    return getApps()[0];
};

export const auth = getAuth(getFirebaseApp());
export const googleProvider = new GoogleAuthProvider();
googleProvider.setCustomParameters({ prompt: 'select_account' });

export const registerWithEmailPassword = async (email: string, password: string) => {
    const credential = await createUserWithEmailAndPassword(auth, email, password);
    await sendEmailVerification(credential.user);
    return credential.user;
};

export const resendVerificationEmail = async (user: User) => {
    await sendEmailVerification(user);
};

export const loginWithEmailPassword = async (email: string, password: string) => {
    const credential = await signInWithEmailAndPassword(auth, email, password);
    await reload(credential.user);
    return credential.user;
};

export const loginWithGooglePopup = async () => {
    await signOut(auth);
    const credential = await signInWithPopup(auth, googleProvider);
    return credential.user;
};

export const sendForgotPasswordEmail = async (email: string) => {
    await sendPasswordResetEmail(auth, email);
};

export const getFirebaseIdToken = async (user: User) => user.getIdToken(true);

export const logoutFirebaseAuth = async () => {
    await signOut(auth);
};
