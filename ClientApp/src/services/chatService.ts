import * as signalR from '@microsoft/signalr';

/**
 * Builds a SignalR HubConnection scoped to /hubs/chat.
 * The JWT token is read from localStorage so authenticated users
 * are identified on the server.
 */
export function buildChatConnection(): signalR.HubConnection {
    const token = localStorage.getItem('token') || '';

    return new signalR.HubConnectionBuilder()
        .withUrl('/hubs/chat', {
            accessTokenFactory: () => token,
        })
        .withAutomaticReconnect()
        .configureLogging(signalR.LogLevel.Warning)
        .build();
}
