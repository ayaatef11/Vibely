import { Injectable } from '@angular/core';
import { environment } from '../../../Environments/environment';
import { HttpClient } from '@angular/common/http';
import { RegisterRequest } from '../../../Models/Auth/Requests/RegisterRequest';
import { catchError, Observable, throwError } from 'rxjs';
import { RegisterResponse } from '../../../Models/Auth/Responses/RegisterResponse';
import { LoginRequest } from '../../../Models/Auth/Requests/LoginRequest';
import { LoginResponse } from '../../../Models/Auth/Responses/LoginResponse';
import { ForgetPasswordResetRequest } from '../../../Models/Auth/Requests/ForgetPasswordResetRequest';
import {jwtDecode} from 'jwt-decode';

@Injectable({ 
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

  ForgetPasswordRequest(email: string)  {
    const encodedEmail = encodeURIComponent(email);
    return this.http.post(`${this.Url}/Forgot-Password-Request?email=${encodedEmail}`, null ) 
  }

  resetPassword(userData: ForgetPasswordResetRequest) {
    return this.http.put(`${this.Url}/Forgot-Password-Reset`, userData )
  }

getUserId(): string | null {
  const token = this.getToken();
  if (!token) return null;

  const decoded: any = jwtDecode(token);

  return decoded.sub || null;
}

getProfileId(): string | null {
  const token = this.getToken();
  if (!token) return null;

  const decoded: any = jwtDecode(token);

  return decoded.ProfileId || null;
}
saveToken(token: string): void {
  localStorage.setItem('token', token);
}

getToken(): string | null {
  return localStorage.getItem('token');
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
