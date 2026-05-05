import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../Environments/environment';
import { UserResponse } from '../../../Models/Users/Responses/UserResponse';

@Injectable({
  providedIn: 'root'
})
export class UserServiceService {
private Url=environment.apiUrl+'User';
  constructor(private http:HttpClient) { }
  reportUser(userId:string,reporterId:string){
return this.http.post(`${this.Url}/report`,{userId,reporterId})
  }

  suggestUser(userId:string){
return this.http.post<UserResponse[]>(`${this.Url}/suggest?userId=${userId}`,{})
  }

  searchUser(keyword:string){
return this.http.post(`${this.Url}/search`,{keyword})
  
  }
}
