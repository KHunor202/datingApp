import { Component, computed, inject, OnInit, signal } from '@angular/core';
import { ActivatedRoute, NavigationEnd, Router, RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { filter, single } from 'rxjs';
import { Member } from '../../../types/member';
import { AgePipe } from '../../../core/pipes/age-pipe';
import { AccountService } from '../../../core/services/account-service';
import { MemberService } from '../../../core/services/member-service';
import { PresenceService } from '../../../core/services/presence-service';
import { LikesService } from '../../../core/services/likes-service';

@Component({
  selector: 'app-member-detailed',
  imports: [ RouterLink, RouterLinkActive, RouterOutlet, AgePipe],
  templateUrl: './member-detailed.html',
  styleUrl: './member-detailed.css'
})
export class MemberDetailed implements OnInit {
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private accountService = inject(AccountService);
  private routeId = signal<string | null>(null);
  protected likesService = inject(LikesService);
  protected memberService = inject(MemberService);
  protected presenceService = inject(PresenceService);
  protected hasLiked = computed(() => {
    return this.likesService.likeIds().includes(this.routeId()!);
  });

  protected title = signal<string | undefined>('Profile');
  protected isCurrentUser = computed(() => {
    return this.accountService.currentUser()?.id === this.route.snapshot.paramMap.get('id');
  });

  constructor() {
    this.route.paramMap.subscribe(params => {
      this.routeId.set(params.get('id'));
    });
  }

  ngOnInit() {
    this.title.set(this.route.snapshot.firstChild?.title);

    this.router.events.pipe(
      filter(event => event instanceof NavigationEnd)
    ).subscribe(() => {
        this.title.set(this.route.snapshot.firstChild?.title);
    });
  }
}
