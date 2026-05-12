import { Component } from '@angular/core';
import { CommonModule, NgFor } from '@angular/common';
import { NotificationResponse } from '../../../../Models/Notifications/Responses/NotificationResponse';
import { AuthenticationService } from '../../../Services/authentication-service.service';
import { NotificationsServiceService } from '../../../Services/notifications-service.service';

@Component({
  selector: 'app-comments-notifications',
  standalone: true,
  imports: [NgFor,CommonModule],
  templateUrl: './comments-notifications.component.html',
  styleUrl: './comments-notifications.component.css'
})
export class CommentsNotificationsComponent {
constructor(private authService:AuthenticationService, private notificationsService:NotificationsServiceService){}
ngOnInit(){
  this.getCommentsNotifications();
}
//**********************VARIABLES******************************************** */
profileId=this.authService.getProfileId()??'1'
notifications!:NotificationResponse[];
//*************************FUNCTIONS************************************************ */
getCommentsNotifications(){
  this.notificationsService.getByType(this.profileId,"Comment").subscribe((res)=>{
this.notifications=res;
  })
}

}
