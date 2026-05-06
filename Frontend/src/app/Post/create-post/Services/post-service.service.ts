import { Injectable } from '@angular/core';
import { environment } from '../../../../Environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { AddPostRequest } from '../../../../Models/Posts/Requests/AddPostRequest';
import { EditPostRequest } from '../../../../Models/Posts/Requests/EditPostRequest';
import { catchError, throwError } from 'rxjs';
import { StartSharePostRequest } from '../../../../Models/Posts/Requests/StartSharePostRequest';
import { RevokeSharePostRequest } from '../../../../Models/Posts/Requests/RevokeSharePostRequest';
import { PostResponse } from '../../../../Models/Posts/Responses/PostResponse';

@Injectable({
  providedIn: 'root'
})
export class PostServiceService {
  private Url = environment.apiUrl + 'post';

  constructor(private http: HttpClient) { }

  addPost(data: AddPostRequest) {
    let params = new HttpParams()
      .set('FeelingState', data.feelingState)
      .set('Title', data.title)
      .set('Text', data.text)
      .set('ProfileId', data.ProfileId);

    const formData = new FormData();

    // data.mediaFiles.forEach(file => {
    //   formData.append('Media', file);
    // });

    return this.http.post<PostResponse>(`${this.Url}/add`, formData, { params });
  }

  editPost(data: EditPostRequest) {
    return this.http.put(`${this.Url}/edit`, data).pipe(
      catchError(this.handleError)
    );
  }

  getUserPosts(profileId: string) {
    let params = new HttpParams()
      .set('profileId', profileId)
    return this.http.get(`${this.Url}/user`, { params }).pipe(
      catchError(this.handleError)
    );
  }

  getPost(postId: string) {
    return this.http.get<PostResponse>(`${this.Url}/${postId}`)
  }

  deletePost(id: string) {
    let params = new HttpParams().set('id', id)
    return this.http.get(`${this.Url}`, { params }).pipe(
      catchError(this.handleError)
    );
  }

  getAllPosts(userId: string) {
    let params = new HttpParams().set('userId', userId)
    return this.http.get<PostResponse[]>(`${this.Url}/get-all`, { params }).pipe(
      catchError(this.handleError)
    );
  }

  searchPosts(keyword: string) {
    let params = new HttpParams()
      .set('keyword', keyword)
    return this.http.get(`${this.Url}/show-posts`, { params }).pipe(
      catchError(this.handleError)
    );
  }

  getTrendingPosts() {
    return this.http.get(`${this.Url}/trending-posts`).pipe(
      catchError(this.handleError)
    );
  }

  getSharesCount(postId: string) {
    let params = new HttpParams()
      .set('postId', postId)
    return this.http.get(`${this.Url}/shares-count`, { params }).pipe(
      catchError(this.handleError)
    );
  }

  getLikesCount(postId: string) {
    let params = new HttpParams()
      .set('postId', postId)
    return this.http.get(`${this.Url}/likes-count`, { params }).pipe(
      catchError(this.handleError)
    );
  }

  hidePost(postId: string) {
    let params = new HttpParams().set('postId', postId)
    return this.http.get(`${this.Url}/hide-post`, { params }).pipe(
      catchError(this.handleError)
    );
  }




  private handleError(error: any) {
    console.error('An error occurred:', error);
    return throwError(() => new Error(
      error.error?.message ||
      error.message ||
      'An unknown error occurred'
    ));
  }
}
