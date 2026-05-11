import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { Observable, Subject } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../Environments/environment';
import { ChatResponse } from '../../../Models/Chats/Responses/ChatResponse';
import { MessageResponse } from '../../../Models/Chats/Responses/MessageResponse';
import { AddMessageRequest } from '../../../Models/Chats/Requests/AddMessageRequest';
import { EditMessageRequest } from '../../../Models/Chats/Requests/EditMessageRequest';
import { AuthenticationService } from '../../Auth/Services/authentication-service.service';

@Injectable({
  providedIn: 'root'
})
export class ChatServiceService {
  constructor(private http:HttpClient){}

//*********************VARIABLES************************** */
private Url=environment.apiUrl+'Chat'
private hubConnection!: signalR.HubConnection;
public messages$ = new Subject<MessageResponse>();

  //**************************FUNCTIONS************************** */
getChats(currentuserId:string):Observable<ChatResponse[]>{
  return this.http.get<ChatResponse[]>(`${this.Url}?currentUserId=${currentuserId}`)
}

getMessages(chatId:string,currentUserId:string):Observable<MessageResponse[]>{
  return this.http.get<MessageResponse[]>(`${this.Url}/messages?chatId=${chatId}&userId=${currentUserId}`)
}

createChat(currentUserId:string,otherUserId:string):Observable<ChatResponse>{
  return this.http.post<ChatResponse>(`${this.Url}/${currentUserId}/${otherUserId}`,{})
}
 
editMessage(messageId:string,request:EditMessageRequest):Observable<MessageResponse>{
  return this.http.put<MessageResponse>(`${this.Url}/edit-message?messageId=${messageId}`,request)
}
deleteMessage(messageId:string,currentUserId:string):Observable<MessageResponse>{
  return this.http.delete<MessageResponse>(`${this.Url}/${messageId}?currentUserId=${currentUserId}`)
}

//******************HUB********************************* */
async startConnection(token: string):Promise<void> {
// debugger
    this.hubConnection = new signalR.HubConnectionBuilder().withUrl(`${environment.hubUrl}chatHub`, {
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

  sendMessage(request:AddMessageRequest):Promise<MessageResponse> {
    return this.hubConnection.invoke<MessageResponse>('SendMessage',request);
  }

  receiveMessage() {

    this.hubConnection.on('ReceiveMessage',(data: MessageResponse) => {
        this.messages$.next(data);
      });
  }
}