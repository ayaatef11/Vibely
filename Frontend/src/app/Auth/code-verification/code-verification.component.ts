import { Component } from '@angular/core';
import { Router, RouterModule ,ActivatedRoute} from '@angular/router';
import { AuthenticationService } from '../Services/authentication-service.service';
import { ForgetPasswordResetRequest } from '../../../Models/Auth/Requests/ForgetPasswordResetRequest';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-code-verification',
  standalone: true,
  imports: [RouterModule,FormsModule],
  templateUrl: './code-verification.component.html',
  styleUrl: './code-verification.component.css'
})
export class CodeVerificationComponent {
  constructor(private authService:AuthenticationService,private router:Router,private route: ActivatedRoute){}
code: string = '';
newPassword: string = '';
confirmPassword: string = '';
email: string = '';

ngOnInit() {
  this.email = this.route.snapshot.queryParams['email'];
}

resetPassword() {

  if (!this.code) {
    alert('Enter code first ');
    return;
  }

  if (this.newPassword !== this.confirmPassword) {
    alert("Passwords don't match ");
    return;
  }

  const request:ForgetPasswordResetRequest = {
    email: this.email,
    code: this.code,
    newPassword: this.newPassword
  };

  this.authService.resetPassword(request).subscribe({
    next: (res:any) => {   
      this.authService.saveToken(res.token)
      this.router.navigate(['/home']);
    },
    error: (err) => {
      console.error(err);
    }
  });
}
}
