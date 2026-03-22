import { useEffect, useState } from 'react';
import * as signalR from '@microsoft/signalr';

export const useSignalR = (bookingId: number | null) => {
  const [connection, setConnection] = useState<signalR.HubConnection | null>(null);
  const [driverLocation, setDriverLocation] = useState<{ latitude: number; longitude: number } | null>(null);

  useEffect(() => {
    if (!bookingId) return;

    const newConnection = new signalR.HubConnectionBuilder()
      .withUrl('/trackingHub')
      .withAutomaticReconnect()
      .build();

    newConnection.start()
      .then(() => {
        console.log('Connected to SignalR!');
        newConnection.invoke('JoinRide', bookingId);
      })
      .catch(err => console.error('SignalR Connection Error: ', err));

    newConnection.on('ReceiveLocation', (data) => {
      setDriverLocation({ latitude: data.latitude, longitude: data.longitude });
    });

    setConnection(newConnection);

    return () => {
      if (newConnection) {
        newConnection.off('ReceiveLocation');
        newConnection.stop();
      }
    };
  }, [bookingId]);

  return { driverLocation, connection };
};
