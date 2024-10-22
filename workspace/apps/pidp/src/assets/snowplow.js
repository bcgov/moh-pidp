// <!-- Snowplow starts plowing - Standalone vE.2.14.0 -->
(function (p, l, o, w, i, n, g) {
  if (!p[i]) {
    p.GlobalSnowplowNamespace = p.GlobalSnowplowNamespace || [];
    p.GlobalSnowplowNamespace.push(i);
    p[i] = function () {
      (p[i].q = p[i].q || []).push(arguments);
    };
    p[i].q = p[i].q || [];
    n = l.createElement(o);
    g = l.getElementsByTagName(o)[0];
    n.async = 1;
    n.src = w;
    g.parentNode.insertBefore(n, g);
  }
})(
  window,
  document,
  'script',
  'https://www2.gov.bc.ca/StaticWebResources/static/sp/sp-2-14-0.js',
  'snowplow',
);
