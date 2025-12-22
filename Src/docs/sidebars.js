/** @type {import('@docusaurus/plugin-content-docs').SidebarsConfig} */
const sidebars = {
  tutorialSidebar: [
    {
      type: 'category',
      label: 'Getting Started',
      items: ['intro', 'quick-start'],
    },
    {
      type: 'category',
      label: 'Guides',
      items: [
        'guides/docker-setup',
        'guides/iis-deployment',
        'guides/postman-collections',
        'guides/docusaurus-openapi-integration',
      ],
    },
  ],
};

export default sidebars;
