import { Component } from '@angular/core';
import { Router, RouterModule, ActivatedRoute } from '@angular/router';
import { ForgetPasswordResetRequest } from '../../../Models/Auth/Requests/ForgetPasswordResetRequest';
import { FormsModule } from '@angular/forms';
import { TranslateModule } from '@ngx-translate/core';
import { AuthenticationService } from '../../Services/authentication-service.service';

@Component({
  selector: 'app-code-verification',
  standalone: true,
  imports: [RouterModule, FormsModule,TranslateModule],
  templateUrl: './code-verification.component.html',
  styleUrl: './code-verification.component.css'
})
export class CodeVerificationComponent {
  constructor(private authService: AuthenticationService, private router: Router, private route: ActivatedRoute) { }

  //*****************************VARIABLES**************************************** */
  data: ForgetPasswordResetRequest = {
    email: '',
    code: '',
    newPassword: '',
    timeOutInMinutes: Number(this.authService.getSessionTimeOut())
  }
  confirmPassword: string = '';

  ngOnInit() {
    this.data.email = this.route.snapshot.queryParams['email'];
  }

  resetPassword() {

    if (!this.data.code) {
      alert('Enter code first ');
      return;
    }

    if (this.data.newPassword !== this.confirmPassword) {
      alert("Passwords don't match ");
      return;
    }


    this.authService.resetPassword(this.data).subscribe({
      next: (res: any) => {
        this.authService.saveToken(res.token)
        this.router.navigate(['/home']);
      },
      error: (err) => {
        console.error(err);
      }
    });
  }
}
