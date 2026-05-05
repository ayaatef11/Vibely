import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../Environments/environment';

@Injectable({
  providedIn: 'root'
})
export class FollowSerivceService {
private Url=environment.apiUrl+'Follow';
  constructor(private http:HttpClient) { }
  requestFollow(){

  }
  acceptFollow(){

  }
  rejectFollow(){

  }
  unfollow(){

  }
  getFollow()[
    
  ]
}
