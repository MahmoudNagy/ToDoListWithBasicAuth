import { Component, OnInit } from '@angular/core';
import { User } from '../../authorization/user.model';
import { AuthorizeService } from '../../authorization/authorize.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent implements OnInit {
  public isAuthenticated: boolean;
  public userName: string;
  currentUser: User;
  isExpanded = false;

  constructor(private router: Router, private authorizeService: AuthorizeService) {
    this.authorizeService.currentUser.subscribe(x => {
      this.currentUser = x;
      if (this.currentUser) {
        this.isAuthenticated = true;
        this.userName = this.currentUser.userName;
      }
      else {
        this.isAuthenticated = false;
        this.userName = null;
      }
    });
  }

  ngOnInit() {
    if (this.currentUser) {
      this.isAuthenticated = true;
      this.userName = this.currentUser.userName;
    }
  }



  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }

  logout() {
    this.authorizeService.logout();
    this.router.navigate(['/login']);
  }

}
