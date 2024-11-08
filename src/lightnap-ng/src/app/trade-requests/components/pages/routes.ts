import { AppRoute } from "@routing";

// TODO: Add this route list to app/routing/routes.ts.
//
// At the top of the file, import the routes:
//
// import { Routes as TradeRequestRoutes } from "../trade-requests/components/pages/routes";
//
// Then add the routes to the list:
//
// { path: "trade-requests", children: TradeRequestRoutes }
//
export const Routes: AppRoute[] = [
  { path: "", loadComponent: () => import("./index/index.component").then(m => m.IndexComponent) },
  { path: "create", loadComponent: () => import("./create/create.component").then(m => m.CreateComponent) },
  { path: ":id", loadComponent: () => import("./get/get.component").then(m => m.GetComponent) },
  { path: ":id/edit", loadComponent: () => import("./edit/edit.component").then(m => m.EditComponent) },
];
