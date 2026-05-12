import { Component } from '@angular/core';
import { NotificationResponse } from '../../../../Models/Notifications/Responses/NotificationResponse';
import { CommonModule, NgFor } from '@angular/common';
import { AuthenticationService } from '../../../Services/authentication-service.service';
import { NotificationsServiceService } from '../../../Services/notifications-service.service';

@Component({
  selector: 'app-likes-notifications',
  standalone: true,
  imports: [NgFor,CommonModule],
  templateUrl: './likes-notifications.component.html',
  styleUrl: './likes-notifications.component.css'
})
export class LikesNotificationsComponent {
constructor(private authService:AuthenticationService, private notificationsService:NotificationsServiceService){}
ngOnInit(){
  this.getLikesNotifications();
}
//**********************VARIABLES******************************************** */
profileId=this.authService.getProfileId()??'1'
notifications!:NotificationResponse[];
//*************************FUNCTIONS************************************************ */
getLikesNotifications(){
  this.notificationsService.getByType(this.profileId,"Like").subscribe((res)=>{
this.notifications=res;
  })
}

}
