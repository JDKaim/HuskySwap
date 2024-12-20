import { inject, Injectable } from "@angular/core";
import { takeUntilDestroyed } from "@angular/core/rxjs-interop";
import { RouteAliasService } from "@routing";
import { MenuItem } from "primeng/api";
import { BehaviorSubject, combineLatest, debounceTime, Subject, tap } from "rxjs";
import { IdentityService } from "src/app/identity/services/identity.service";
import { MenuChangeEvent } from "../models/menu-change-event";

@Injectable({
  providedIn: "root",
})
export class MenuService {
  #routeAlias = inject(RouteAliasService);
  #identityService = inject(IdentityService);

  #menuSource = new Subject<MenuChangeEvent>();
  menuSource$ = this.#menuSource.asObservable();

  #defaultMenuItems = new Array<MenuItem>({
    label: "Dashboard",
    items: [{ label: "Home", icon: "pi pi-fw pi-home", routerLink: this.#routeAlias.getRoute("user-home") },
            { label: "All Classes", icon: "pi pi-fw pi-book", routerLink: this.#routeAlias.getRoute("class-infos") },
            { label: "My Classes", icon: "pi pi-fw pi-book", routerLink: this.#routeAlias.getRoute("my-classes") },
            { label: "My Wishlist", icon: "pi pi-fw pi-gift", routerLink: this.#routeAlias.getRoute("wishlist") },
            { label: "Incoming Swaps", icon: "pi pi-fw pi-search", routerLink: this.#routeAlias.getRoute("incoming-swaps") },
            { label: "Outgoing Swaps", icon: "pi pi-fw pi-sync", routerLink: this.#routeAlias.getRoute("outgoing-swaps") },
            { label: "Completed Swaps", icon: "pi pi-fw pi-verified", routerLink: this.#routeAlias.getRoute("completed-swaps") }]
  });

  #adminMenuItems = new Array<MenuItem>({
    label: "Admin",
    items: [
      { label: "Home", icon: "pi pi-fw pi-home", routerLink: this.#routeAlias.getRoute("admin-home") },
      { label: "Users", icon: "pi pi-fw pi-users", routerLink: this.#routeAlias.getRoute("admin-users") },
      { label: "Roles", icon: "pi pi-fw pi-lock", routerLink: this.#routeAlias.getRoute("admin-roles") },
    ],
  });

  #menuItemSubject = new BehaviorSubject<Array<MenuItem>>(this.#defaultMenuItems);

  #isLoggedIn = false;
  #isAdminLoggedIn = false;

  constructor() {
    combineLatest([
      this.#identityService.watchLoggedIn$().pipe(tap(isLoggedIn => (this.#isLoggedIn = isLoggedIn))),
      this.#identityService.watchLoggedInToRole$("Administrator").pipe(tap(isAdminLoggedIn => (this.#isAdminLoggedIn = isAdminLoggedIn))),
    ])
      .pipe(takeUntilDestroyed(), debounceTime(100))
      .subscribe(() => this.#refreshMenuItems());
  }

  onMenuStateChange(event: MenuChangeEvent) {
    this.#menuSource.next(event);
  }

  #refreshMenuItems() {
    var menuItems = [...this.#defaultMenuItems];

    if (this.#isAdminLoggedIn) {
      menuItems.push(...this.#adminMenuItems);
    }

    this.#menuItemSubject.next(menuItems);
  }

  watchMenuItems$() {
    return this.#menuItemSubject.asObservable();
  }
}
