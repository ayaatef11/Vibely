import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { LoginRequest } from '../../../../Models/Auth/Requests/LoginRequest';
import { AuthenticationService } from '../../Services/authentication-service.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [RouterModule,FormsModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
userData:LoginRequest={
userName:'',
password:''
}
  isLoading = false;
  errorMessage = '';
constructor(private router:Router, private authService:AuthenticationService){}

onSubmit(): void {
  // debugger
    this.isLoading = true;
    this.errorMessage = '';
this.authService.SignIn(this.userData).subscribe({
  next:(response)=>{
   console.log('Signin successful', response);
    localStorage.setItem('token', response.token);
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