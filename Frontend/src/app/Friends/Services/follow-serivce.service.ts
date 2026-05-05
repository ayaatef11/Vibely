import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../Environments/environment';
import { FollowRequest } from '../../../Models/Follow/Requests/FollowRequest';
import { AcceptFollowRequest } from '../../../Models/Follow/Requests/AcceptFollowRequest';
import { RejectFollowRequest } from '../../../Models/Follow/Requests/RejectFollowRequest';
import { UnFollowRequest } from '../../../Models/Follow/Requests/UnFollowRequest';
import { UserResponse } from '../../../Models/Users/Responses/UserResponse';

@Injectable({
  providedIn: 'root'
})
export class FollowSerivceService {
private Url=environment.apiUrl+'Follow';
  constructor(private http:HttpClient) { }
  requestFollow(data:FollowRequest){
return this.http.post(`${this.Url}/request`,data)
  }
  unrequestFollow(data:FollowRequest){
return this.http.post(`${this.Url}/unrequest`,data)
  }

  acceptFollow(data:AcceptFollowRequest){
return this.http.put(`${this.Url}/accept`,data)
  }

  rejectFollow(data:RejectFollowRequest){
return this.http.put(`${this.Url}/reject`,data)
  }
  
  unfollow(data:UnFollowRequest){
return this.http.delete(`${this.Url}/unfollow`,{body:data})
  }
  getFollow(id:string){
    return this.http.get(`${this.Url}/Get/${id}`)
  }
   getFollowingWithStories(id:string){
    return this.http.get(`${this.Url}/Get-following/${id}`)
  }
  viewRequests(userId:string){
    return this.http.get<UserResponse[]>(`${this.Url}/view?userId=${userId}`)
  }
}
