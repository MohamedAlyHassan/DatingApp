import { take } from 'rxjs/operators';
import { User } from 'src/app/_models/user';
import { Directive, Input, TemplateRef, ViewContainerRef, OnInit } from '@angular/core';
import { AccountService } from '../_services/account.service';

@Directive({
  selector: '[appHasRole]'
})
export class HasRoleDirective implements OnInit {
  @Input() appHasRole: string[];
  user: User;

  constructor(private viewContainerRef: ViewContainerRef, private temlateRef: TemplateRef<any>,
    private accountService: AccountService) { 
      this.accountService.currentUser$.pipe(take(1)).subscribe(user => {
        this.user = user
      })
    }
  ngOnInit(): void {
    if (!this.user?.roles || this.user == null) {
      this.viewContainerRef.clear();
      return;
    }

    if (this.user?.roles.some(r => this.appHasRole.includes(r))) {
      this.viewContainerRef.createEmbeddedView(this.temlateRef);
    } else {
      this.viewContainerRef.clear();
    }
  }

}
