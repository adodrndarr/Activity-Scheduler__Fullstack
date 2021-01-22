import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { AuthService } from '../auth/auth.service';


@Component({
  selector: 'app-header',
  templateUrl: './header.component.html'
})
export class HeaderComponent implements OnInit, OnDestroy {
  constructor(private authService: AuthService) { }


  isAdmin = false;
  userSub: Subscription;
  isLoggedIn = false;
  toggleDropdown = true;

  ngOnInit() {
    this.checkUserRole();
  }

  private checkUserRole(): void {
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

  toggleMenu(): void {
    this.toggleDropdown = !this.toggleDropdown;
  }

  ngOnDestroy(): void {
    this.userSub.unsubscribe();
  }
}
