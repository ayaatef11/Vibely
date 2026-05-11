import { Injectable } from '@angular/core';
import { environment } from '../../../Environments/environment';
import { HttpClient } from '@angular/common/http';
import { ReelsRequest } from '../../../Models/Reels/Requests/ReelsRequest';

@Injectable({
  providedIn: 'root'
})
export class ReelsServiceService {
  private Url = environment.apiUrl + 'Reels'
  constructor(private http: HttpClient) { }
  
  UploadStory(request: ReelsRequest) {
    this.http.post(`${this.Url}/Upload`, request)
  }

  GetStoryById(storyId: string) {
    this.http.get(`${this.Url}/Get/${storyId}`)
  }

  DeleteStory(storyId: string) {
    this.http.delete(`${this.Url}/Remove/${storyId}`)
  }

}
