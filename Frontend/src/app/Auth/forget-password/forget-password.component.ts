import { Component } from '@angular/core';
import { AuthenticationService } from '../Services/authentication-service.service';
import { Router, RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { TranslateModule } from '@ngx-translate/core';

@Component({
  selector: 'app-forget-password',
  standalone: true,
  imports: [RouterModule,FormsModule,TranslateModule],
  templateUrl: './forget-password.component.html',
  styleUrl: './forget-password.component.css'
})
export class ForgetPasswordComponent {
  constructor(private authService:AuthenticationService,private router:Router){}

  //**************************VARIABLES********************************** */
email: string = '';

//*************************FUNCTIONS***************** */
requestReset() {
  debugger
  this.authService.ForgetPasswordRequest(this.email).subscribe({
    next: () => {
      alert('Code sent to your email ');
      this.router.navigate(['/verify-code'], {
        queryParams: { email: this.email }
      });
    },
    error: (err) => {
      console.error(err);
      alert('Error sending code');
    }
  });
}
}
