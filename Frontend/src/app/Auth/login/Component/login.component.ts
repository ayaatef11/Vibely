import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { LoginRequest } from '../../../../Models/Auth/Requests/LoginRequest';
import { AuthenticationService } from '../../Services/authentication-service.service';
import { TokenResponse } from '../../../../Models/Auth/Responses/TokenResponse';
import { NgIf } from '@angular/common';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [RouterModule, FormsModule,NgIf],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  constructor(private router: Router, private authService: AuthenticationService) { }

  //***************************VARIABLES********************* */
  userData: LoginRequest = {
    userName: '',
    password: '',
    timeOutInMinutes: Number(this.authService.getSessionTimeOut()),
    isLoginNotificationsEnabled: this.authService.getNotificationsSettings() === 'true'
  }
  userId = this.authService.getUserId() ?? '1'
  isLoading = false;
  errorMessage = '';
  is2FALoginModalOpen!: boolean
  verificationCode!: string
  //*****************************FUNCTIONS***************************************** */
  onSubmit(): void {
    // debugger
    this.isLoading = true;
    this.errorMessage = '';
    this.authService.SignIn(this.userData).subscribe({
      next: (res: TokenResponse) => {

        if (res.requires2FA) {
          this.open2FALoginModal(this.userId);
        } else {
          this.authService.saveToken(res.token);
          this.isLoading = false;
          this.router.navigate(['/home']);
        }
      },
      error: (error) => {
        console.error('Signin error', error);
        this.errorMessage = error.message || 'Signin failed';
        this.isLoading = false;
      }
    })
  }

  open2FALoginModal(userId: string) {
    this.userId = userId;
    this.is2FALoginModalOpen = true;
  }
  verifyLogin2FA() {

    const data = {
      userId: this.userId,
      code: this.verificationCode
    };

    this.authService.verify2FA(data).subscribe(res => {
      this.authService.saveToken(res.token);

      this.is2FALoginModalOpen = false;
      this.isLoading = false;
      this.router.navigate(['/home']);
    });

  }
}