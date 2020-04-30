FROM prom/prometheus

USER root

COPY docker-entrypoint.sh /usr/local/bin/
RUN chmod +x /usr/local/bin/docker-entrypoint.sh

COPY prometheus.yml /etc/prometheus/prometheus.yml

# USER nobody

ENTRYPOINT ["/usr/local/bin/docker-entrypoint.sh"]