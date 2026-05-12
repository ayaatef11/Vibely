import { Component } from '@angular/core';
import { NotificationResponse } from '../../../../Models/Notifications/Responses/NotificationResponse';
import { CommonModule, NgFor } from '@angular/common';
import { AuthenticationService } from '../../../Services/authentication-service.service';
import { NotificationsServiceService } from '../../../Services/notifications-service.service';

@Component({
  selector: 'app-un-read-notifications',
  standalone: true,
  imports: [NgFor,CommonModule],
  templateUrl: './un-read-notifications.component.html',
  styleUrl: './un-read-notifications.component.css'
})
export class UnReadNotificationsComponent {
constructor(private notificationsService:NotificationsServiceService,private authService:AuthenticationService){}
ngOnInit(){
  this.getUnreadNotifications();
}

//*****************VARIABLES******************************* */  
profileId=this.authService.getProfileId()??'1'
notifications!:NotificationResponse[];
//************************FUNCTIONS******************************* */
markAllAsRead(): void {
  this.notificationsService.markAllAsRead(this.profileId).subscribe(()=>{
this.notifications=[]

  });
}

getUnreadNotifications(){
this.notificationsService.getUnread(this.profileId).subscribe((res)=>{
  this.notifications=res;
});
}

}
