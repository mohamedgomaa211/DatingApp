import { AccountService } from 'src/app/_services/account.service';
import { UserParams } from './../../_models/userParams';
import { Component, OnInit } from '@angular/core';
import { Member } from 'src/app/_models/member';
import { Pagination } from 'src/app/_models/pagination';
import { MembersService } from 'src/app/_services/members.service';
import { take } from 'rxjs';
import { User } from 'src/app/_models/User';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css']
})
export class MemberListComponent implements OnInit {
  members: Member[] = [];
  pagination: Pagination | undefined;
  userParams:UserParams|undefined;
  user:User|undefined;
  genderList = [{ value: 'male', display: 'Males' }, { value: 'female', display: 'Females' }]


  constructor(private memberService: MembersService,private accountService:AccountService) {
    this.accountService.currentUser$.pipe(take(1)).subscribe({
      next: user=>{
        if(user){
          this.userParams=new UserParams(user);
          this.user=user;
        }

      }
    })

  }
  ngOnInit(): void {
    this.loadMembers();
  }
  resetFilters() {
    if(this.user){
      this.userParams = new UserParams(this.user);
      this.loadMembers();
    }
  }
  loadMembers() {
    if(!this.userParams) return;
    this.memberService.getMembers(this.userParams).subscribe({
      next: response => {
        if (response.result && response.pagination) {
          this.members = response.result;
          this.pagination = response.pagination;
        }
      }

    })

  }
  pagedChanged(event:any){
    if(this.userParams&& this.userParams?.pageNumber!==event.page){
      this.userParams.pageNumber=event.page;
      this.loadMembers();

    }
  }
}
