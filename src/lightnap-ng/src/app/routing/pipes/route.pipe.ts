import { Pipe, PipeTransform, inject } from "@angular/core";
import { RouteAliasService } from "@routing/services/route-alias-service";
import { RouteAlias } from "@routing/models/route-alias";

@Pipe({
  name: "route",
  standalone: true,
})
/**
 * A pipe that transforms a given `RouteAlias` into route data that can be bound to [routerLink] in templates.
 */
export class RoutePipe implements PipeTransform {
    #routeAliasService = inject(RouteAliasService);

    /**
     * Transforms the provided `RouteAlias` into a route string.
     *
     * @param view - The `RouteAlias` to be transformed.
     * @param value - Optional additional value to be used in the transformation.
     * @returns The route string corresponding to the provided `RouteAlias`.
     */
    transform(view: RouteAlias, value?: any) {
        return this.#routeAliasService.getRoute(view, value);
    }
}
