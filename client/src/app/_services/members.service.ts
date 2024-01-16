import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment.development';
import { Member } from '../_models/member';
import { map, of } from 'rxjs';
import { PaginatedResult } from '../_models/pagination';
import { UserParams } from '../_models/userParams';

@Injectable({
  providedIn: 'root'
})
export class MembersService {
  baseUrl = environment.apiUrl;
  members: Member[] = [];
  memberCache=new Map();


  constructor(private http: HttpClient) { }
  getMembers(userParams:UserParams) {
    let params = this.GetPaginationHeaders(userParams.pageNumber, userParams.PageSize);
    const response=this.memberCache.get(Object.values(params).join('-'));
    if(response) return of (response);
    console.log(Object.values(params).join('_'));

    params=params.append('minAge',userParams.minAge);
    params=params.append('maxAge',userParams.maxAge);
    params=params.append('gender',userParams.gender);
    params=params.append('orderBy',userParams.orderBy);


    return this.GetPaginationResult<Member[]>(this.baseUrl + 'users',params).pipe(map(response=>{
      this.memberCache.set(Object.values(params).join('-'),response)
      return response;
    }))


  }
  private GetPaginationResult<T>(url:string,params: HttpParams) {
   const pagintedResult:PaginatedResult<T>=new PaginatedResult<T>;
    return this.http.get<T>(url, { observe: 'response', params }).pipe(
      map(response => {
        if (response.body) {
          pagintedResult.result = response.body;
        }
        const pagination = response.headers.get('Pagination');
        if (pagination) {
          pagintedResult.pagination = JSON.parse(pagination);
        }
        return pagintedResult;
      })
    );
  }

  private GetPaginationHeaders(pageNumber:number,PageSize:number) {
    let params = new HttpParams();
      params = params.append('pageNumber', pageNumber);
      params = params.append('pageSize', PageSize);
    return params;
  }

  getMember(username: string) {
    const member = this.members.find(x => x.userName === username);
    if (member !== undefined) return of(member);
    return this.http.get<Member>(this.baseUrl + 'users/' + username);
  }

  setMainPhoto(photoId: number) {
    return this.http.put(this.baseUrl + 'users/set-main-photo/' + photoId, {});
  }
  deletePhoto(photoId: number) {
    return this.http.delete(this.baseUrl + 'users/delete-photo/' + photoId, {});
  }
  updateMember(member: Member) {
    return this.http.put<Member>(this.baseUrl + 'users', member).pipe(
      map(() => {
        const index = this.members.indexOf(member);
        this.members[index] = { ...this.members[index], ...member }
      })
    );
  }
}
