import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { Subject } from 'rxjs';
import { ChatMessage } from '../../../Models/Chats/ChatMessage';

@Injectable({
  providedIn: 'root'
})
export class ChatServiceService {

  private hubConnection!: signalR.HubConnection;

  public messages$ = new Subject<ChatMessage>();

async startConnection(token: string):Promise<void> {

    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl('https://localhost:7042/chatHub', {
        accessTokenFactory: () => token
      })
      .withAutomaticReconnect()
      .build();

    await this.hubConnection.start()
      .then(() => {
        console.log('SignalR Connected');
      })
      .catch(err => console.log(err));

    this.receiveMessage();
  }

  sendMessage(receiverId: string, message: string) {

    return this.hubConnection.invoke('SendMessage',receiverId,message);
  }

  receiveMessage() {

    this.hubConnection.on(
      'ReceiveMessage',
      (data: ChatMessage) => {

        this.messages$.next(data);
      });
  }
}