import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core'; 
import { environment } from '../../Environments/environment';
import { AddCommentRequest } from '../../Models/Comments/Requests/AddCommentRequest';
import { DislikeCommentRequest } from '../../Models/Comments/Requests/DislikeCommentRequest';
import { EditCommentRequest } from '../../Models/Comments/Requests/EditCommentRequest';
import { LikeCommentRequest } from '../../Models/Comments/Requests/LikeCommentRequest';
import { CommentResponse } from '../../Models/Posts/Responses/CommentResponse';

@Injectable({
  providedIn: 'root'
})
export class CommentServiceService {
private Url=environment.apiUrl+'Comment'

  constructor(private http:HttpClient) { }
  addComment(data:AddCommentRequest){
return this.http.post<CommentResponse>(`${this.Url}/add`,data);
  }

  likeComment(data:LikeCommentRequest){
    return this.http.post<CommentResponse>(`${this.Url}/like`,data);
  }

  editComment(data:EditCommentRequest){
    return this.http.put<CommentResponse>(`${this.Url}/edit`,data)
  }

  dislikeComment(data:DislikeCommentRequest){
    return this.http.delete<CommentResponse>(`${this.Url}/dislike`,{body:data});
  }

  deleteComment(id:string){
    return this.http.delete(`${this.Url}/delete/${id}`);
  }
  
  getComment(postId:string){
    return this.http.get<CommentResponse[]>(`${this.Url}/${postId}`);
  }
}
