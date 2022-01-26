import { Component, OnInit } from '@angular/core';
import { AuthService } from 'src/app/auth/auth.service';
import { User } from 'src/app/auth/Entities/Models/user.model';


@Component({
  selector: 'app-user-account',
  templateUrl: './user-account.component.html'
})
export class UserAccountComponent implements OnInit {
  constructor(
    private authService: AuthService
  ) { }


  user: User;

  ngOnInit(): void {
    const currentUser = this.authService.user.value;
    if (currentUser)
      this.user = currentUser;
  }
}
