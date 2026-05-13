import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { catchError, Observable, throwError } from 'rxjs'; 
import { environment } from '../../Environments/environment';
import { AddPostRequest } from '../../Models/Posts/Requests/AddPostRequest';
import { EditPostRequest } from '../../Models/Posts/Requests/EditPostRequest';
import { PostResponse } from '../../Models/Posts/Responses/PostResponse';

@Injectable({
  providedIn: 'root'
})
export class PostServiceService {
  private Url = environment.apiUrl + 'post';

  constructor(private http: HttpClient) { }

  addPost(post: AddPostRequest): Observable<PostResponse> {
    const formData = new FormData();
  formData.append('ProfileId', post.ProfileId);
  formData.append('title', post.title);
  formData.append('text', post.text);
  formData.append('feelingState', post.feelingState.toString());

  // Append each file with the same key "media"
  if (post.media && post.media.length > 0) {
    post.media.forEach(file => formData.append('media', file));
  }
    return this.http.post<PostResponse>(`${this.Url}/add`, formData);
  }

  editPost(data: EditPostRequest): Observable<PostResponse> {
    return this.http.put<PostResponse>(`${this.Url}/edit`, data);
  }

  getUserPosts(profileId: string): Observable<PostResponse[]> {
    let params = new HttpParams().set('profileId', profileId)
    return this.http.get<PostResponse[]>(`${this.Url}/user`, { params });
  }

  getPost(postId: string,profileId:string): Observable<PostResponse> {
    return this.http.get<PostResponse>(`${this.Url}/${postId}/${profileId}`)
  }

  deletePost(id: string) {
    return this.http.delete(`${this.Url}/${id}`);
  }

  getAllPosts(profileId: string): Observable<PostResponse[]> {
    let params = new HttpParams().set('profileId', profileId)
    return this.http.get<PostResponse[]>(`${this.Url}/get-all`, { params });
  }

  searchPosts(keyword: string): Observable<PostResponse[]> {
    let params = new HttpParams().set('keyword', keyword)
    return this.http.get<PostResponse[]>(`${this.Url}/search-posts`, { params });
  }

  getTrendingPosts(): Observable<PostResponse[]> {
    return this.http.get<PostResponse[]>(`${this.Url}/trending-posts`);
  }

  getSharesCount(postId: string): Observable<number> {
    let params = new HttpParams().set('postId', postId)
    return this.http.get<number>(`${this.Url}/shares-count`, { params });
  }

  getLikesCount(postId: string): Observable<number> {
    let params = new HttpParams().set('postId', postId)
    return this.http.get<number>(`${this.Url}/likes-count`, { params });
  }

  hidePost(postId: string) {
    let params = new HttpParams().set('postId', postId)
    return this.http.get(`${this.Url}/hide-post`, { params });
  }


}
