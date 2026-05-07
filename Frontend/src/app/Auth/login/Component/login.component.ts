import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { LoginRequest } from '../../../../Models/Auth/Requests/LoginRequest';
import { AuthenticationService } from '../../Services/authentication-service.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [RouterModule, FormsModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  constructor(private router: Router, private authService: AuthenticationService) { }

  //***************************VARIABLES********************* */
  userData: LoginRequest = {
    userName: '',
    password: '',
    timeOutInMinutes:Number(this.authService.getSessionTimeOut()),
    isLoginNotificationsEnabled:this.authService.getNotificationsSettings()==='true'
  }
  isLoading = false;
  errorMessage = '';

  //*****************************FUNCTIONS***************************************** */
  onSubmit(): void {
    // debugger
    this.isLoading = true;
    this.errorMessage = '';
    this.authService.SignIn(this.userData).subscribe({
      next: (res) => {
this.authService.saveToken(res.token)
        this.isLoading = false;
        this.router.navigate(['/home']);
      },
      error: (error) => {
        console.error('Signin error', error);
        this.errorMessage = error.message || 'Signin failed';
        this.isLoading = false;
      }
    })

  }
}