import { Component, OnInit } from '@angular/core';
import { RouterModule } from '@angular/router';
import { NotificationsServiceService } from '../Services/notifications-service.service';
import { AuthenticationService } from '../../../Auth/Services/authentication-service.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-notigications-header',
  standalone: true,
  imports: [RouterModule,CommonModule],
  templateUrl: './notifications-header.component.html',
  styleUrl: './notifications-header.component.css'
})
export class NotificationsHeaderComponent implements OnInit{

  constructor(private notificationService: NotificationsServiceService,private authService: AuthenticationService) {}
   unreadCount$ = this.notificationService.unreadCount$;
  profileId=this.authService.getProfileId()??'1';
 token=this.authService.getToken()??'';
  ngOnInit(): void { 
    this.notificationService.startConnection(this.token);
    this.notificationService.loadAll(this.profileId);
  }
 
  markAllAsRead(): void {
    this.notificationService.markAllAsRead(this.profileId).subscribe(() => {
      this.notificationService.loadAll(this.profileId);
    });
  }
}
