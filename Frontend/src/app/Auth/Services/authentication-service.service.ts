import { Injectable } from '@angular/core';
import { environment } from '../../../Environments/environment';
import { HttpClient } from '@angular/common/http';
import { RegisterRequest } from '../../../Models/Auth/Requests/RegisterRequest';
import { catchError, Observable, throwError, timeout } from 'rxjs';
import { LoginRequest } from '../../../Models/Auth/Requests/LoginRequest';
import { ForgetPasswordResetRequest } from '../../../Models/Auth/Requests/ForgetPasswordResetRequest';
import { jwtDecode } from 'jwt-decode';
import { ChangePasswordRequest } from '../../../Models/Auth/Requests/ChangePasswordRequest';
import { SessionResponse } from '../../../Models/Auth/Responses/SessionResponse';
import { TokenResponse } from '../../../Models/Auth/Responses/TokenResponse';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {
  private Url = environment.apiUrl + 'Authentication';
  constructor(private http: HttpClient) { }

  signUp(userData: RegisterRequest): Observable<TokenResponse> {
    return this.http.post<TokenResponse>(`${this.Url}/signUp`, userData).pipe(
      catchError(this.handleError)
    );
  }

  SignIn(userData: LoginRequest): Observable<TokenResponse> {
    return this.http.post<TokenResponse>(`${this.Url}/login`, userData).pipe(
      catchError(this.handleError)
    )
  }

  changePassword(data: ChangePasswordRequest, userId: string) {
    return this.http.post(`${this.Url}/change-password?userId=${userId}`, data)
  }

  ForgetPasswordRequest(email: string) {
    const encodedEmail = encodeURIComponent(email);
    return this.http.post(`${this.Url}/Forgot-Password-Request?email=${encodedEmail}`, null)
  }

  resetPassword(userData: ForgetPasswordResetRequest) {
    return this.http.put(`${this.Url}/Forgot-Password-Reset`, userData)
  }

  changeSessionTimeOut(userId: string, timeOut: number): Observable<SessionResponse> {
    return this.http.get<SessionResponse>(`${this.Url}/change-session-timeout?userId=${userId}&timeOut=${timeOut}`)
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
  getUserName(): string | null {
    const token = this.getToken();
    if (!token) return null;

    const decoded: any = jwtDecode(token);

    return decoded.UserName || null;
  }
  saveToken(token: string): void {
    localStorage.setItem('token', token);
  }
SaveSessionTimeOut(timeOut: string): void {
    localStorage.setItem('session-time-out', timeOut);
  }

  getToken(): string | null {
    return localStorage.getItem('token');
  }
   getSessionTimeOut(): string | null {
    return localStorage.getItem('session-time-out');
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
