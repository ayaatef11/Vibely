import { Component, OnInit } from '@angular/core';
import { Contact } from '../../../Models/Chats/Contact';
import { ChatMessage } from '../../../Models/Chats/ChatMessage';
import { ChatServiceService } from '../Services/chat-service.service';
import { FormsModule } from '@angular/forms';
import { CommonModule, NgClass } from '@angular/common';
import { AuthenticationService } from '../../Auth/Services/authentication-service.service';
 

@Component({
  selector: 'app-friend-chat',
  standalone: true,
  imports: [FormsModule,NgClass,CommonModule],
  templateUrl: './friend-chat.component.html',
  styleUrl: './friend-chat.component.css'
})
export class FriendChatComponent {

  contacts: Contact[] = [];
  messages: ChatMessage[] = [];
  selectedContact!: Contact;
  newMessage = '';
currentUserId=this.authService.getUserId()??'1'
  constructor(private chatService: ChatServiceService,private authService:AuthenticationService) { }

  async ngOnInit():Promise<void> {
    const token = this.authService.getToken()??'1';
    await this.chatService.startConnection(token);

    this.chatService.messages$.subscribe(message => {
        this.messages.push(message);
      });

    // fake data for now
    this.contacts = [
      {
        id: '1',
        name: 'James',
        imageUrl: 'profile.jpg',
        lastMessage: 'Hello',
        isOnline: true
      },
      {
        id: '2',
        name: 'Karen',
        imageUrl: 'profile.jpg',
        lastMessage: 'How are you',
        isOnline: false
      }
    ];
  }

  selectContact(contact: Contact) {

    this.selectedContact = contact;

    // later:
    // load messages from API
  }

  sendMessage() {
debugger
    if (!this.newMessage.trim()) return;

    this.chatService.sendMessage(this.selectedContact.id, this.newMessage);

    // add locally immediately
    this.messages.push({
      senderId: 'current-user',
      receiverId: this.selectedContact.id,
      content: this.newMessage,
      sentAt: new Date()
    });

    this.newMessage = '';
  }

}
