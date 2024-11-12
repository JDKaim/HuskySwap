import { inject } from "@angular/core";
import { ActivatedRouteSnapshot, createUrlTreeFromSnapshot, RouterStateSnapshot } from "@angular/router";
import { RouteAliasService } from "@routing";
import { map, take } from "rxjs";
import { IdentityService } from "src/app/identity/services/identity.service";

export const authGuard = (next: ActivatedRouteSnapshot, state: RouterStateSnapshot) => {
  const routeAliasService = inject(RouteAliasService);
  const identityService = inject(IdentityService);

  return identityService
    .watchLoggedIn$()
    .pipe(
      take(1),
      map(isLoggedIn => {
        if (isLoggedIn) return true;
        identityService.redirectUrl = state.url;
        return createUrlTreeFromSnapshot(next, routeAliasService.getRoute("login"));
      })
    );
};
