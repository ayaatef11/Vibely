import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AuthenticationService } from '../../../Auth/Services/authentication-service.service';
import { SessionResponse } from '../../../../Models/Auth/Responses/SessionResponse';

@Component({
  selector: 'app-privacy-and-security',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './privacy-and-security.component.html',
  styleUrl: './privacy-and-security.component.css'
})
export class PrivacyAndSecurityComponent {
constructor(private authService:AuthenticationService){}
ngOnInit(){
  this.selectedTimeOut=Number.parseInt(this.authService.getSessionTimeOut()??'30',10);
 
  this.selectedLoginNotifications =this.authService.getNotificationsSettings() === 'true';
}
//****************VARIABLES************************************ */
userId=this.authService.getUserId()??'1'
selectedTimeOut!:number;
selectedLoginNotifications!:boolean;
///**********************FUNCTIONS**************************** */
changeSessionTimeOut(){ 
  this.authService.changeSessionTimeOut(this.userId,this.selectedTimeOut).subscribe(
    (res:SessionResponse)=>{
    this.selectedTimeOut=res.timeOut
    this.authService.SaveSessionTimeOut(res.timeOut.toString());
    this.authService.saveToken(res.token);
  })
}

changeNotificationsSettings(){
  this.authService.saveLoginNotificationsSettings(this.selectedLoginNotifications.toString())
}

}
