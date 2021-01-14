import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { AuthService } from '../auth/auth.service';
import { CookieService } from 'ngx-cookie-service';


@Component({
  selector: 'app-header',
  templateUrl: './header.component.html'
})
export class HeaderComponent implements OnInit, OnDestroy {
  constructor(private authService: AuthService, private cookieService: CookieService) { }


  isAdmin = false;
  userSub: Subscription;
  isLoggedIn = false;

  ngOnInit() {
    this.userSub = this.authService.user
      .subscribe(user => {
        if (user) {
          this.isAdmin = user.isAdmin;
          this.isLoggedIn = true;
        }
        else {
          this.isAdmin = false;
          this.isLoggedIn = false;
        }
      });
  }

  onLogout(): void {
    this.authService.logout();
  }

  ngOnDestroy(): void {
    this.userSub.unsubscribe();
  }
}
