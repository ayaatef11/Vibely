import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../Environments/environment';
import { BlockRequest, UnBlockRequest } from '../../Models/Blocks/Requests/BlockRequest';

@Injectable({
  providedIn: 'root'
})
export class BlocksServiceService {
private Url=environment.apiUrl+'Blocks';
  constructor(private http:HttpClient) { }
  blockUser(data:BlockRequest){
    return this .http.post(`${this.Url}/block`,data)
  }
  unblockUser(data:UnBlockRequest){
    return this.http.delete(`${this.Url}/unblock`,{body:data})
  }
  getBlockedUsers(blockerId:string){
    return this.http.get(`${this.Url}/GetBlockedUsers/${blockerId}`)
  }
}
