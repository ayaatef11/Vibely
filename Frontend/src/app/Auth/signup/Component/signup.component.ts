import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import { Router, RouterModule } from '@angular/router';
import { RegisterRequest } from '../../../../Models/Auth/Requests/RegisterRequest';
import { AuthenticationService } from '../../Services/authentication-service.service';
import { NgIf } from '@angular/common';
@Component({
  selector: 'app-signup',
  standalone: true,
  imports: [FormsModule,RouterModule,NgIf],
  templateUrl: './signup.component.html',
  styleUrl: './signup.component.css'
})
export class SignupComponent {
  signUpData:RegisterRequest={
  userName: '',
  fullName: '',
  email: '',
  password: '',
  confirmPassword:'',
  location: '' 
  }
  isLoading = false;
  errorMessage = '';

constructor(private authService:AuthenticationService,private router:Router){}

onSubmit():void{
    this.isLoading = true;
    this.errorMessage = '';

    this.authService.signUp(this.signUpData).subscribe({
      next: (response) => {
        console.log('Signup successful', response);
         localStorage.setItem('token', response.token);
        this.isLoading = false;
        this.router.navigate(['/login']);  
      },
      error: (error) => {
        console.error('Signup error', error);
        this.errorMessage = error.message || 'Signup failed';
        this.isLoading = false;
      }
    });
  }
}
