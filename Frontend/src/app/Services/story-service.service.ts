import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core'; 
import { Observable } from 'rxjs';  
import { environment } from '../../Environments/environment';
import { AddStoryCommentRequest } from '../../Models/Story/Requests/AddStoryCommentRequest';
import { DeleteStoryRequest } from '../../Models/Story/Requests/DeleteStoryRequest';
import { StoryCommentResponse } from '../../Models/Story/Responses/StoryCommentResponse';
import { StoryResponse } from '../../Models/Story/Responses/StoryResponse';

@Injectable({
  providedIn: 'root'
})
export class StoryServiceService {
  private Url = environment.apiUrl + 'Story';
  constructor(private http: HttpClient) { }
  getAll(userId: string):Observable<StoryResponse[]> {
    return this.http.get<StoryResponse[]>(`${this.Url}/get-all?userId=${userId}`)
  }

  viewStory(userId: string, storyId: string):Observable<StoryResponse> {
    return this.http.get<StoryResponse>(`${this.Url}/view?userId=${userId}&storyId=${storyId}`)
  }

  getViewers(userId: string, storyId: string) {
    return this.http.get(`${this.Url}/get-viewers/${userId}/${storyId}`)
  }

  addReact(userId: string, storyId: string) {
    return this.http.post(`${this.Url}/add-react?userId=${userId}&storyId=${storyId}`, {});
  }

  addStoryComment(data: AddStoryCommentRequest):Observable<StoryCommentResponse> {
    return this.http.post<StoryCommentResponse>(`${this.Url}/add-comment`, data)
  }

  addStory(text: string, userId: string) :Observable<StoryResponse>{
    return this.http.post<StoryResponse>(`${this.Url}/add?Text=${text}&ProfileId=${userId}`,{})
  }

  getUserStories(profileId: string) {
    return this.http.get(`${this.Url}/user/${profileId}`)
  }

  deleteStory(data: DeleteStoryRequest) {
    return this.http.delete(`${this.Url}/delete`, { body: data })

  }
}
