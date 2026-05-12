import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule, NgClass } from '@angular/common';
import { ChatResponse } from '../../../Models/Chats/Responses/ChatResponse';
import { MessageResponse } from '../../../Models/Chats/Responses/MessageResponse';
import { AddMessageRequest } from '../../../Models/Chats/Requests/AddMessageRequest';
import { EditMessageRequest } from '../../../Models/Chats/Requests/EditMessageRequest';
import { ActivatedRoute } from '@angular/router';
import { AuthenticationService } from '../../Services/authentication-service.service';
import { ChatServiceService } from '../../Services/chat-service.service';
import { ToastrService } from 'ngx-toastr';


@Component({
  selector: 'app-friend-chat',
  standalone: true,
  imports: [FormsModule, NgClass, CommonModule],
  templateUrl: './friend-chat.component.html',
  styleUrl: './friend-chat.component.css'
})
export class FriendChatComponent {
  constructor(private chatService: ChatServiceService, private toastService:ToastrService,private route:ActivatedRoute,private authService: AuthenticationService) { }

  async ngOnInit(): Promise<void> {
    const token = this.authService.getToken() ?? '1';
    await this.chatService.startConnection(token);

    this.chatService.getChats(this.currentUserId).subscribe((res) => {
      this.chats = res;
      const chatId=this.route.snapshot.paramMap.get('chatId')
      if(chatId){
        const found=this.chats.find(c=>c.id==chatId);
        if(found)this.selectChat(found)
      }
    })

     this.chatService.messages$.subscribe(message => {
      this.messages.push(message);
      this.selectedChat.lastMessage = message.content

    });
  }
  //**************************VARIABLES******************************** */
  chats: ChatResponse[] = [];
  messages: MessageResponse[] = [];
  selectedChat!: ChatResponse;
  newMessage = '';
  currentUserId = this.authService.getUserId() ?? '1'

  //****************************FUNCTIONS************************************** */  
  selectChat(chat: ChatResponse) {
    // debugger
    this.selectedChat = chat;
    this.chatService.getMessages(chat.id, this.currentUserId).subscribe((res) => {
      this.messages = res;
    })
  }

  sendMessage() {
    // debugger
    if (!this.newMessage.trim()) return;
    const req: AddMessageRequest = {
      chatId: this.selectedChat.id,
      senderId: this.currentUserId,
      receiverId: this.selectedChat.participantId,
      content: this.newMessage
    }
    this.chatService.sendMessage(req);
    this.newMessage = '';
  }

editMessage(message: MessageResponse) {
  const updatedContent = prompt('Edit your message', message.content);

  if (!updatedContent || updatedContent.trim() === '') return;
const req:EditMessageRequest={
  currentUserId:this.currentUserId,
  newContent:updatedContent
}
  this.chatService.editMessage(message.id, req).subscribe((res:MessageResponse) => {
    message.content = res.content;
  });
}

deleteMessage(message: MessageResponse) {

  this.chatService.deleteMessage(message.id, this.currentUserId)
    .subscribe((res:MessageResponse) => {
     message.content = res.content;
    });
}

}
