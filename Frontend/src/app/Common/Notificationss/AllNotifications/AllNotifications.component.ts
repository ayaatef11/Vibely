import { Component, OnInit } from '@angular/core';
import { RouterModule } from '@angular/router';
import { NotificationResponse } from '../../../../Models/Notifications/Responses/NotificationResponse';
import { NotificationsServiceService } from '../Services/notifications-service.service';
import { AuthenticationService } from '../../../Auth/Services/authentication-service.service';
import { CommonModule, NgFor, NgIf } from '@angular/common';

@Component({
  selector: 'app-allNotifications',
  standalone: true,
  imports: [RouterModule,NgFor,NgIf,CommonModule],
  templateUrl: './AllNotifications.component.html',
  styleUrl: './AllNotifications.component.css'
})
export class AllNotificationsComponent implements OnInit{

  constructor( private notificationService: NotificationsServiceService,private authService: AuthenticationService) {}
 
  ngOnInit(): void { 
    this.notificationService.notifications$.subscribe(n => (this.notifications = n));
  }
 //*****************VARIABLES******************** */
  notifications: NotificationResponse[] = [];
  private userId = this.authService.getUserId()??'';
 //************************FUNCTIONS********************************************* */
  markAsRead(notification: NotificationResponse): void {
    if (notification.isRead) return;
    this.notificationService.markAsRead(notification.id).subscribe(() => {
      notification.isRead = true;
    });
  }
}
