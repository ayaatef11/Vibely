import { Component } from '@angular/core';
import { ChangePasswordRequest } from '../../../../Models/Auth/Requests/ChangePasswordRequest';
import { FormsModule } from '@angular/forms';
import { TranslateModule } from '@ngx-translate/core';
import { AuthenticationService } from '../../../Services/authentication-service.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-account',
  standalone: true,
  imports: [FormsModule,TranslateModule],
  templateUrl: './account.component.html',
  styleUrl: './account.component.css'
})
export class AccountComponent {
  constructor(private authService:AuthenticationService,private toastService:ToastrService){}
  userId=this.authService.getUserId()??'1'

  model:ChangePasswordRequest={
oldPassword:'',
newPassword:'',
confirmNewPassword:''
}
changePassword() {
this.authService.changePassword(this.model,this.userId).subscribe(
  (next)=>{
  this.model={
    oldPassword:'',
    newPassword:'',
    confirmNewPassword:''
  }
},
(err)=>{
this.toastService.error(err)
});
}

}
