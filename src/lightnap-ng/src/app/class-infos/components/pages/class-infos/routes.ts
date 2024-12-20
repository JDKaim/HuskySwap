
import { adminGuard } from "@core/guards/admin.guard";
import { AppRoute } from "@routing";

// TODO: Add this route list to app/routing/routes.ts.
//
// At the top of the file, import the routes:
//
// import { Routes as ClassInfoRoutes } from "../class-infos/components/pages/routes";
//
// Then add the routes to the list:
//
// { path: "class-infos", children: ClassInfoRoutes }
//
export const Routes: AppRoute[] = [
  { path: "", data: {alias: "class-infos"}, loadComponent: () => import("./index/index.component").then(m => m.IndexComponent) },
  { path: "create", canActivate: [adminGuard], loadComponent: () => import("./create/create.component").then(m => m.CreateComponent) },
  { path: ":id", data: {alias: "class-info"}, loadComponent: () => import("./get/get.component").then(m => m.GetComponent) },
  { path: ":id/edit", loadComponent: () => import("./edit/edit.component").then(m => m.EditComponent) },
];
