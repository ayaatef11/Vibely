import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core'; 
import { environment } from '../../../Environments/environment';
import { LikeRequest } from '../../../Models/Reacts/Requests/LikeRequest';
import { DislikeRequest } from '../../../Models/Reacts/Requests/DislikeRequest';

@Injectable({
  providedIn: 'root'
})
export class LikesServiceService {
  private Url = environment.apiUrl + 'Like'
  constructor(private http: HttpClient) { }
  likePost(data: LikeRequest) {
    return this.http.post(`${this.Url}/like`, data)
  }
  
  dislikePost(data: DislikeRequest) {
    return this.http.delete(`${this.Url}/dislike`, { body: data })
  }
}
