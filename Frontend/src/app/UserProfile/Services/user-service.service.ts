import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../Environments/environment';

@Injectable({
  providedIn: 'root'
})
export class UserServiceService {
private Url=environment.apiUrl+'User';
  constructor(private http:HttpClient) { }
  reportUser(userId:string,reporterId:string){
this.http.post(`${this.Url}/report`,{userId,reporterId})
  }
  suggestUser(userId:string){
this.http.post(`${this.Url}/suggest`,{userId})
  }
  searchUser(keyword:string){
this.http.post(`${this.Url}/search`,{keyword})
  
  }
}
