import { Injectable } from '@angular/core';
import { environment } from '../../../Environments/environment';
import { HttpClient } from '@angular/common/http';
import { RegisterRequest } from '../../../Models/Auth/Requests/RegisterRequest';
import { catchError, Observable, throwError } from 'rxjs';
import { RegisterResponse } from '../../../Models/Auth/Responses/RegisterResponse';
import { LoginRequest } from '../../../Models/Auth/Requests/LoginRequest';
import { LoginResponse } from '../../../Models/Auth/Responses/LoginResponse';
import { ForgetPasswordResetRequest } from '../../../Models/Auth/Requests/ForgetPasswordResetRequest';
import { ForgetPasswordResetResponse } from '../../../Models/Auth/Responses/ForgetPasswordResetResponse';

@Injectable({//make this service global
  providedIn: 'root'
})
export class AuthenticationService {
  private Url = environment.apiUrl + 'Authentication';
  constructor(private http: HttpClient) { }

  signUp(userData: RegisterRequest): Observable<RegisterResponse> {
    return this.http.post<RegisterResponse>(
      `${this.Url}/signUp`, userData
    ).pipe(
      catchError(this.handleError)
    );
  }

  SignIn(userData: LoginRequest): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(
      `${this.Url}/login`, userData
    ).pipe(
      catchError(this.handleError)
    )
  }

  ForgetPasswordRequest(email: string): Observable<string> {
    const encodedEmail = encodeURIComponent(email);
    return this.http.post<string>(
      `${this.Url}/Forgot-Password-Request?email=${encodedEmail}`, null
    ).pipe(
      catchError(this.handleError)
    )
  }

  ForgetPasswordReset(userData: ForgetPasswordResetRequest): Observable<ForgetPasswordResetResponse> {
    return this.http.put<ForgetPasswordResetResponse>(
      `${this.Url}/Forgot-Password-Reset`, userData
    ).pipe(
      catchError(this.handleError)
    )
  }

  private handleError(error: any) {//to filter the error
    console.error('An error occurred:', error);
    return throwError(() => new Error(
      error.error?.message ||
      error.message ||
      'An unknown error occurred during signup'
    ));
  }
}
