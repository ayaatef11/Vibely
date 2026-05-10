import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../Environments/environment';
import { FollowRequest } from '../../../Models/Follow/Requests/FollowRequest';
import { AcceptFollowRequest } from '../../../Models/Follow/Requests/AcceptFollowRequest';
import { RejectFollowRequest } from '../../../Models/Follow/Requests/RejectFollowRequest';
import { UnFollowRequest } from '../../../Models/Follow/Requests/UnFollowRequest';
import { UserResponse } from '../../../Models/Users/Responses/UserResponse';
import { ProfileResponse } from '../../../Models/Profiles/Responses/ProfileResponse';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class FollowSerivceService {
  private Url = environment.apiUrl + 'Follow';
  constructor(private http: HttpClient) { }

  acceptFollow(data: AcceptFollowRequest) {
    return this.http.put(`${this.Url}/accept`, data)
  }

  requestFollow(data: FollowRequest): Observable<ProfileResponse> {
    return this.http.post<ProfileResponse>(`${this.Url}/request`, data)
  }

  unrequestFollow(data: FollowRequest): Observable<ProfileResponse> {
    return this.http.post<ProfileResponse>(`${this.Url}/unrequest`, data)
  }



  rejectFollow(data: RejectFollowRequest): Observable<ProfileResponse> {
    return this.http.put<ProfileResponse>(`${this.Url}/reject`, data)
  }

  unfollow(data: UnFollowRequest): Observable<ProfileResponse> {
    return this.http.delete<ProfileResponse>(`${this.Url}/unfollow`, { body: data })
  }

  getFollow(id: string) {
    return this.http.get(`${this.Url}/Get/${id}`)
  }

  getFollowingWithStories(id: string) {
    return this.http.get(`${this.Url}/Get-following/${id}`)
  }

  viewRequests(userId: string) {
    return this.http.get<UserResponse[]>(`${this.Url}/view?userId=${userId}`)
  }
}
