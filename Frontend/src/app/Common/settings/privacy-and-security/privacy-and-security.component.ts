import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AuthenticationService } from '../../../Auth/Services/authentication-service.service';
import { SessionResponse } from '../../../../Models/Auth/Responses/SessionResponse';
import { Verify2FARequest } from '../../../../Models/Auth/Requests/Verify2FARequest';
import { NgIf } from '@angular/common';
import { EnableTwoFAResponse } from '../../../../Models/Auth/Responses/EnableTwoFAResponse';
import QRCode from 'qrcode';
import { TranslateModule } from '@ngx-translate/core';
@Component({
  selector: 'app-privacy-and-security',
  standalone: true,
  imports: [FormsModule,NgIf,TranslateModule],
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
is2FAEnabled: boolean = false;
is2FAModalOpen: boolean = false;
verificationCode: string = '';
enable2faData:EnableTwoFAResponse={
  qrCodeUrl:'',
  secret:''
}
qrImage: string = '';
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

toggle2FA() {
  if (this.is2FAEnabled) {
    this.enable2FAFlow();
  } else {
    this.disable2FA();
  }
}

enable2FAFlow() {
  this.authService.enable2FA(this.userId).subscribe((res:EnableTwoFAResponse) => {
    
    console.log("2FA enabled response:", res);
 
    this.open2FAVerificationModal(res);
  });
}
verify2FASetup() {
  const data: Verify2FARequest = {
    userId: this.userId,
    code: this.verificationCode
  };

  this.authService.verify2FASetup(data).subscribe(res => {
    console.log("2FA verified:", res);
    this.is2FAEnabled = true;
  });
}
disable2FA() {//need backend
  this.is2FAEnabled = false;
}

open2FAVerificationModal(res: EnableTwoFAResponse) {
  this.enable2faData = res;

  QRCode.toDataURL(res.qrCodeUrl)
    .then(url => {
      this.qrImage = url;
      this.is2FAModalOpen = true;
    });
}
close2FAModal() {
  this.is2FAModalOpen = false;
  this.verificationCode = '';
}

}
