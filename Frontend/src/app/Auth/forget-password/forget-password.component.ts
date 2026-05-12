import { Component } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { TranslateModule } from '@ngx-translate/core';
import { AuthenticationService } from '../../Services/authentication-service.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-forget-password',
  standalone: true,
  imports: [RouterModule,FormsModule,TranslateModule],
  templateUrl: './forget-password.component.html',
  styleUrl: './forget-password.component.css'
})
export class ForgetPasswordComponent {
  constructor(private authService:AuthenticationService,private router:Router,private toastr:ToastrService){}

  //**************************VARIABLES********************************** */
email: string = '';

//*************************FUNCTIONS***************** */
requestReset() {
  debugger
  this.authService.ForgetPasswordRequest(this.email).subscribe({
    next: () => {
      this.toastr.success('Code sent to your email ');
      this.router.navigate(['/verify-code'], {
        queryParams: { email: this.email }
      });
    },
    error: (err) => {
     this.toastr.error('Error sending code');
    }
  });
}
}
